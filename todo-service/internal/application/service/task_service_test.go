package service

import (
	"testing"
	"time"

	"todo-service/internal/application/dto"
	"todo-service/internal/domain/entity"

	"github.com/google/uuid"
)

type memoryTaskRepository struct {
	task *entity.Task
}

func (r *memoryTaskRepository) Create(task *entity.Task) error {
	r.task = task
	return nil
}

func (r *memoryTaskRepository) GetByID(string) (*entity.Task, error) {
	return r.task, nil
}

func (r *memoryTaskRepository) GetAll() ([]*entity.Task, error) {
	return []*entity.Task{r.task}, nil
}

func (r *memoryTaskRepository) Update(task *entity.Task) error {
	r.task = task
	return nil
}

func (r *memoryTaskRepository) Delete(string) error {
	r.task = nil
	return nil
}

func TestUpdateTaskPreservesEntityIdentityAndUpdatesFields(t *testing.T) {
	createdAt := time.Date(2025, time.January, 1, 0, 0, 0, 0, time.UTC)
	taskID := uuid.New()
	repo := &memoryTaskRepository{
		task: &entity.Task{
			BaseEntity: entity.BaseEntity{
				ID:        taskID,
				CreatedAt: createdAt,
				UpdatedAt: createdAt,
			},
			Title: "Old title",
		},
	}
	taskService := NewTaskService(repo)
	dueDate := time.Date(2025, time.December, 31, 0, 0, 0, 0, time.UTC)

	err := taskService.UpdateTask(repo.task.ID.String(), &dto.UpdateTaskRequest{
		Title:       "Updated title",
		Description: "Updated description",
		IsCompleted: true,
		DueDate:     dueDate,
	})
	if err != nil {
		t.Fatalf("UpdateTask() error = %v", err)
	}

	if repo.task.ID != taskID {
		t.Errorf("ID = %v, want %v", repo.task.ID, taskID)
	}
	if !repo.task.CreatedAt.Equal(createdAt) {
		t.Errorf("CreatedAt = %v, want %v", repo.task.CreatedAt, createdAt)
	}
	if repo.task.Title != "Updated title" || repo.task.Description != "Updated description" {
		t.Errorf("updated task fields = (%q, %q), want (%q, %q)",
			repo.task.Title, repo.task.Description, "Updated title", "Updated description")
	}
	if !repo.task.IsCompleted || !repo.task.DueDate.Equal(dueDate) {
		t.Errorf("completion/due date = (%v, %v), want (true, %v)",
			repo.task.IsCompleted, repo.task.DueDate, dueDate)
	}
	if !repo.task.UpdatedAt.After(createdAt) {
		t.Errorf("UpdatedAt = %v, want later than %v", repo.task.UpdatedAt, createdAt)
	}
}

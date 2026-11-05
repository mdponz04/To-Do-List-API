package service

import (
	"errors"
	"time"

	"todo-service/internal/application/dto"
	"todo-service/internal/domain/entity"
	"todo-service/internal/domain/interface/repository"
)

type TaskService struct {
	taskRepo repository.TaskRepository
}

func NewTaskService(taskRepo repository.TaskRepository) *TaskService {
	return &TaskService{
		taskRepo: taskRepo,
	}
}

func (s *TaskService) CreateTask(task *dto.CreateTaskRequest) error {
	if task == nil {
		return errors.New("task request cannot be nil")
	}

	return s.taskRepo.Create(
		entity.NewTask(task.Title, task.Description, task.DueDate),
	)
}

func (s *TaskService) GetTaskByID(id string) (*dto.TaskResponse, error) {
	task, err := s.taskRepo.GetByID(id)
	if err != nil {
		return nil, err
	}
	return toTaskResponse(task), nil
}

func (s *TaskService) GetAllTasks() ([]*dto.TaskResponse, error) {
	tasks, err := s.taskRepo.GetAll()
	if err != nil {
		return nil, err
	}
	returnDtos := make([]*dto.TaskResponse, len(tasks))
	for i, task := range tasks {
		returnDtos[i] = toTaskResponse(task)
	}
	return returnDtos, nil
}

func (s *TaskService) UpdateTask(id string, request *dto.UpdateTaskRequest) error {
	if request == nil {
		return errors.New("task request cannot be nil")
	}

	task, err := s.taskRepo.GetByID(id)
	if err != nil {
		return err
	}

	task.Title = request.Title
	task.Description = request.Description
	task.IsCompleted = request.IsCompleted
	task.DueDate = request.DueDate
	task.UpdatedAt = time.Now()

	return s.taskRepo.Update(task)
}

func (s *TaskService) DeleteTask(id string) error {
	return s.taskRepo.Delete(id)
}

func toTaskResponse(task *entity.Task) *dto.TaskResponse {
	return &dto.TaskResponse{
		ID:          task.ID,
		CreatedAt:   task.CreatedAt,
		UpdatedAt:   task.UpdatedAt,
		Title:       task.Title,
		Description: task.Description,
		IsCompleted: task.IsCompleted,
		DueDate:     task.DueDate,
	}
}

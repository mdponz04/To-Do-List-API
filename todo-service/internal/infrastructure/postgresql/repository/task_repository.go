package repository

import (
	"database/sql"
	"todo-service/internal/domain/entity"
)

type TaskRepository struct {
	db *sql.DB
}

func NewTaskRepository(db *sql.DB) *TaskRepository {
	return &TaskRepository{
		db: db,
	}
}

func (r *TaskRepository) Create(task *entity.Task) error {
	query := `
        INSERT INTO tasks (
            id,
            title,
            description,
            is_completed,
            due_date,
            created_at,
            updated_at
        )
        VALUES ($1, $2, $3, $4, $5, $6, $7)
    `

	_, err := r.db.Exec(
		query,
		task.ID,
		task.Title,
		task.Description,
		task.IsCompleted,
		task.DueDate,
		task.CreatedAt,
		task.UpdatedAt,
	)

	return err
}

func (r *TaskRepository) GetByID(id string) (*entity.Task, error) {
	query := `
		SELECT id, title, description, is_completed, due_date, created_at, updated_at
		FROM tasks
		WHERE id = $1
	`
	var task entity.Task
	err := r.db.QueryRow(query, id).Scan(
		&task.ID,
		&task.Title,
		&task.Description,
		&task.IsCompleted,
		&task.DueDate,
		&task.CreatedAt,
		&task.UpdatedAt,
	)
	if err != nil {
		return nil, err
	}
	return &task, nil
}

func (r *TaskRepository) GetAll() ([]*entity.Task, error) {
	query := `
		SELECT id, title, description, is_completed, due_date, created_at, updated_at
		FROM tasks
		ORDER BY created_at DESC
	`
	rows, err := r.db.Query(query)
	if err != nil {
		return nil, err
	}
	defer rows.Close()

	tasks := make([]*entity.Task, 0)
	for rows.Next() {
		var task entity.Task
		err := rows.Scan(
			&task.ID,
			&task.Title,
			&task.Description,
			&task.IsCompleted,
			&task.DueDate,
			&task.CreatedAt,
			&task.UpdatedAt,
		)
		if err != nil {
			return nil, err
		}
		tasks = append(tasks, &task)
	}
	if err := rows.Err(); err != nil {
		return nil, err
	}
	return tasks, nil
}

func (r *TaskRepository) Update(task *entity.Task) error {
	query := `
		UPDATE tasks
		SET title = $1, description = $2, is_completed = $3, due_date = $4, updated_at = $5
		WHERE id = $6
	`
	result, err := r.db.Exec(
		query,
		task.Title,
		task.Description,
		task.IsCompleted,
		task.DueDate,
		task.UpdatedAt,
		task.ID,
	)
	if err != nil {
		return err
	}
	rowsAffected, err := result.RowsAffected()
	if err != nil {
		return err
	}
	if rowsAffected == 0 {
		return sql.ErrNoRows
	}
	return nil
}

func (r *TaskRepository) Delete(id string) error {
	query := `
		DELETE FROM tasks
		WHERE id = $1
	`
	result, err := r.db.Exec(query, id)
	if err != nil {
		return err
	}
	rowsAffected, err := result.RowsAffected()
	if err != nil {
		return err
	}
	if rowsAffected == 0 {
		return sql.ErrNoRows
	}
	return nil
}

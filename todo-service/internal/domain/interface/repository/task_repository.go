package repository

import "todo-service/internal/domain/entity"

type TaskRepository interface {
	Create(task *entity.Task) error
	GetByID(id string) (*entity.Task, error)
	GetAll() ([]*entity.Task, error)
	Update(task *entity.Task) error
	Delete(id string) error
}

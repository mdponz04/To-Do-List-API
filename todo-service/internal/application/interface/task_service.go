package service

import "todo-service/internal/application/dto"

type TaskService interface {
	CreateTask(task *dto.CreateTaskRequest) error
	GetTaskByID(id string) (*dto.TaskResponse, error)
	GetAllTasks() ([]*dto.TaskResponse, error)
	UpdateTask(id string, task *dto.UpdateTaskRequest) error
	DeleteTask(id string) error
}

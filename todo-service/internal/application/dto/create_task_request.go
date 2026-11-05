package dto

import "time"

type CreateTaskRequest struct {
	Title       string    `json:"title" binding:"required"`
	Description string    `json:"description"`
	DueDate     time.Time `json:"due_date"`
}

type UpdateTaskRequest struct {
	Title       string    `json:"title" binding:"required"`
	Description string    `json:"description"`
	IsCompleted bool      `json:"is_completed"`
	DueDate     time.Time `json:"due_date"`
}

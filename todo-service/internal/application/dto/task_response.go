package dto

import (
	"time"

	"github.com/google/uuid"
)

type TaskResponse struct {
	ID          uuid.UUID `json:"id"`
	CreatedAt   time.Time `json:"created_at"`
	UpdatedAt   time.Time `json:"updated_at"`
	Title       string    `json:"title"`
	Description string    `json:"description"`
	IsCompleted bool      `json:"is_completed"`
	DueDate     time.Time `json:"due_date"`
}

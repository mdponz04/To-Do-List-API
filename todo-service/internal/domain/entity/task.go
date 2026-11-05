package entity

import "time"

type Task struct {
	BaseEntity
	
	Title       string
	Description string
	IsCompleted	bool
	DueDate		time.Time
}

func NewTask(title string, description string, dueDate time.Time) *Task {
	return &Task{
		BaseEntity: NewBaseEntity(),
		Title:       title,
		Description: description,
		IsCompleted: false,
		DueDate:     dueDate,
	}
}

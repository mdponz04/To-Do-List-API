package entity

import (
	"time"

	"github.com/google/uuid"
)

type BaseEntity struct {
	ID        uuid.UUID
	CreatedAt time.Time
	UpdatedAt time.Time
}

func NewBaseEntity() BaseEntity {
	return BaseEntity{
		ID:        uuid.New(),
		CreatedAt: time.Now(),
		UpdatedAt: time.Now(),
	}
}
package main

import (
	"log"
	"todo-service/internal/api"
	"todo-service/internal/application/service"
	"todo-service/internal/infrastructure/postgresql"
	postgresqlRepository "todo-service/internal/infrastructure/postgresql/repository"

	_ "todo-service/cmd/docs"

	"github.com/joho/godotenv"
)

// @title To-Do List API
// @version 1.0
// @description API for creating, viewing, updating, and deleting tasks.
// @BasePath /
func main() {
	if err := godotenv.Load(); err != nil {
		log.Fatal("Error loading .env")
	}

	db, err := postgresql.NewPostgresConnection()

	if err != nil {
		log.Fatal("Database connection failed:", err)
	}

	defer db.Close()

	taskRepo := postgresqlRepository.NewTaskRepository(db)
	taskService := service.NewTaskService(taskRepo)
	taskHandler := api.NewTaskHandler(taskService)

	router := api.SetupRouter(taskHandler)

	log.Println("Application started on :8080")
	if err := router.Run(":8080"); err != nil {
		log.Fatal("Application server failed: ", err)
	}
}

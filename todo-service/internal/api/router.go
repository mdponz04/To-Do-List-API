package api

import (
	"github.com/gin-gonic/gin"
	swaggerFiles "github.com/swaggo/files"
	ginSwagger "github.com/swaggo/gin-swagger"
)

func SetupRouter(taskHandler *TaskHandler) *gin.Engine {
	router := gin.Default()

	router.GET("/swagger/*any", ginSwagger.WrapHandler(swaggerFiles.Handler))

	tasks := router.Group("/tasks")
	tasks.POST("", taskHandler.CreateTask)
	tasks.GET("", taskHandler.GetAllTasks)
	tasks.GET("/:id", taskHandler.GetTaskByID)
	tasks.PUT("/:id", taskHandler.UpdateTask)
	tasks.DELETE("/:id", taskHandler.DeleteTask)

	return router
}

package api

import (
	"database/sql"
	"errors"
	"net/http"
	"todo-service/internal/application/dto"
	service "todo-service/internal/application/interface"

	"github.com/gin-gonic/gin"
	"github.com/google/uuid"
)

type TaskHandler struct {
	taskService service.TaskService
}

func NewTaskHandler(taskService service.TaskService) *TaskHandler {
	return &TaskHandler{
		taskService: taskService,
	}
}

// CreateTask godoc
// @Summary Create a task
// @Description Creates a new task.
// @Tags tasks
// @Accept json
// @Produce json
// @Param task body dto.CreateTaskRequest true "Task details"
// @Success 201 {object} map[string]string "Task created successfully"
// @Failure 400 {object} map[string]string "Invalid request body"
// @Failure 500 {object} map[string]string "Failed to create task"
// @Router /tasks [post]
func (h *TaskHandler) CreateTask(c *gin.Context) {
	var req dto.CreateTaskRequest

	if err := c.ShouldBindJSON(&req); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	if err := h.taskService.CreateTask(&req); err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": "Failed to create task"})
		return
	}

	c.JSON(http.StatusCreated, gin.H{"message": "Task created successfully"})
}

// GetAllTasks godoc
// @Summary List all tasks
// @Description Returns all tasks.
// @Tags tasks
// @Produce json
// @Success 200 {array} dto.TaskResponse
// @Failure 500 {object} map[string]string "Failed to retrieve tasks"
// @Router /tasks [get]
func (h *TaskHandler) GetAllTasks(c *gin.Context) {
	tasks, err := h.taskService.GetAllTasks()
	if err != nil {
		c.JSON(http.StatusInternalServerError, gin.H{"error": "Failed to retrieve tasks"})
		return
	}

	c.JSON(http.StatusOK, tasks)
}

// GetTaskByID godoc
// @Summary Get a task
// @Description Returns a task by its UUID.
// @Tags tasks
// @Produce json
// @Param id path string true "Task UUID"
// @Success 200 {object} dto.TaskResponse
// @Failure 400 {object} map[string]string "Invalid task ID"
// @Failure 404 {object} map[string]string "Task not found"
// @Failure 500 {object} map[string]string "Failed to retrieve task"
// @Router /tasks/{id} [get]
func (h *TaskHandler) GetTaskByID(c *gin.Context) {
	id, ok := parseTaskID(c)
	if !ok {
		return
	}

	task, err := h.taskService.GetTaskByID(id)
	if err != nil {
		writeTaskError(c, err, "Failed to retrieve task")
		return
	}

	c.JSON(http.StatusOK, task)
}

// UpdateTask godoc
// @Summary Update a task
// @Description Updates an existing task by its UUID.
// @Tags tasks
// @Accept json
// @Produce json
// @Param id path string true "Task UUID"
// @Param task body dto.UpdateTaskRequest true "Updated task details"
// @Success 200 {object} map[string]string "Task updated successfully"
// @Failure 400 {object} map[string]string "Invalid task ID or request body"
// @Failure 404 {object} map[string]string "Task not found"
// @Failure 500 {object} map[string]string "Failed to update task"
// @Router /tasks/{id} [put]
func (h *TaskHandler) UpdateTask(c *gin.Context) {
	id, ok := parseTaskID(c)
	if !ok {
		return
	}

	var req dto.UpdateTaskRequest
	if err := c.ShouldBindJSON(&req); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	if err := h.taskService.UpdateTask(id, &req); err != nil {
		writeTaskError(c, err, "Failed to update task")
		return
	}

	c.JSON(http.StatusOK, gin.H{"message": "Task updated successfully"})
}

// DeleteTask godoc
// @Summary Delete a task
// @Description Deletes a task by its UUID.
// @Tags tasks
// @Param id path string true "Task UUID"
// @Success 204 "Task deleted successfully"
// @Failure 400 {object} map[string]string "Invalid task ID"
// @Failure 404 {object} map[string]string "Task not found"
// @Failure 500 {object} map[string]string "Failed to delete task"
// @Router /tasks/{id} [delete]
func (h *TaskHandler) DeleteTask(c *gin.Context) {
	id, ok := parseTaskID(c)
	if !ok {
		return
	}

	if err := h.taskService.DeleteTask(id); err != nil {
		writeTaskError(c, err, "Failed to delete task")
		return
	}

	c.Status(http.StatusNoContent)
}

func parseTaskID(c *gin.Context) (string, bool) {
	id := c.Param("id")
	if _, err := uuid.Parse(id); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": "Invalid task ID"})
		return "", false
	}
	return id, true
}

func writeTaskError(c *gin.Context, err error, message string) {
	if errors.Is(err, sql.ErrNoRows) {
		c.JSON(http.StatusNotFound, gin.H{"error": "Task not found"})
		return
	}
	c.JSON(http.StatusInternalServerError, gin.H{"error": message})
}

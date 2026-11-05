package postgresql

import (
	"database/sql"
	"fmt"
	"os"

	_ "github.com/jackc/pgx/v5/stdlib"
)

func NewPostgresConnection() (*sql.DB, error) {
	dataSourceName := fmt.Sprintf(
		"postgres://%s:%s@%s:%s/%s",
		os.Getenv("DB_USER"),
		os.Getenv("DB_PASSWORD"),
		os.Getenv("DB_HOST"),
		os.Getenv("DB_PORT"),
		os.Getenv("DB_NAME"),
	)
	
	db, err := sql.Open("pgx",dataSourceName)

	if err != nil {
		return nil, err
	}

	err = db.Ping();

	if err != nil {
		return nil, err
	}

	return db, nil
}
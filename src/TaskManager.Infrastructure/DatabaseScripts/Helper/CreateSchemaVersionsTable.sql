CREATE TABLE IF NOT EXISTS schemaversions
(
    schemaversionsid SERIAL PRIMARY KEY,
    scriptname       VARCHAR(255) NOT NULL,
    applied          TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
CREATE DATABASE stock_db;

USE stock_db;

CREATE TABLE IF NOT EXISTS stock_db.funds_history
(id int auto_increment, date DATE, amount int, memo varchar(1024), type tinyint, index(id));

Create TABLE IF NOT EXISTS stock_db.funds
(capital int, profit int);

Create TABLE IF NOT EXISTS stock_db.stock_transaction_history
(id int auto_increment, code int, date DATE, quantity int, price double, type tinyint, memo varchar(1024), index(id));

Create TABLE IF NOT EXISTS stock_db.stock_code
(code int, name varchar(128));

Create TABLE IF NOT EXISTS stock_db.holding_stock
(id int auto_increment, code int, date DATE, quantity int, price double, index(id));

Create TABLE IF NOT EXISTS stock_db.sold_stock
(id int auto_increment, code int, bought_date DATE, sold_date DATE, quantity int, profit int, index(id));

Create TABLE IF NOT EXISTS stock_db.investment_trust_transaction_history
(id int auto_increment, code int, name varchar(128), date DATE, quantity int, price double, unit int, type tinyint, memo varchar(1024), index(id));

INSERT INTO stock_db.funds (capital, profit) VALUES (0, 0);
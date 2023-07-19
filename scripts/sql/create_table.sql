CREATE TABLE IF NOT EXISTS stock_db.funds_hisotry
(id int auto_increment, date DATE, amount int, memo varchar(1024), type tinyint, index(id));

Create TABLE IF NOT EXISTS stock_db.funds
(capital int, profit int);

Create TABLE IF NOT EXISTS stock_db.stock_transaction_history
(id int auto_increment, code int, date DATE, amount int, price double, type tinyint, memo varchar(1024), index(id));

Create TABLE IF NOT EXISTS stock_db.stock_code
(code int, name varchar(128));

Create TABLE IF NOT EXISTS stock_db.holding_stock
(id int auto_increment, code int, date DATE, amount int, price double, index(id));

Create TABLE IF NOT EXISTS stock_db.sold_stock
(id int auto_increment, code int, bought_date DATE, sold_date DATE, amount int, profit int, index(id));

Create TABLE IF NOT EXISTS stock_db.dividend
(id int auto_increment, code int, date DATE, profit int, index(id));

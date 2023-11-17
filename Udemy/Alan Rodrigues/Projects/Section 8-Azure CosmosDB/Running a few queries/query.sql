SELECT * FROM Orders 

SELECT * FROM Orders o WHERE o.category="Laptop"

SELECT o.orderId,o.category,o.quantity FROM Orders o WHERE o.category="Laptop"

SELECT SUM(o.quantity) AS Quantity,o.category 
FROM Orders o
GROUP BY o.category
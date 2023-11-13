SELECT o.orderId,o.category,o.quantity,o.customer FROM Orders o WHERE o.category="Laptop"

SELECT o.orderId,o.category,o.quantity,o.customer.customerName FROM Orders o WHERE o.category="Laptop"
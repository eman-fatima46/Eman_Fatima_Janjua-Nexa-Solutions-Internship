USE LibraryDb_Week3;
GO

INSERT INTO Authors (AuthorId, FullName)
VALUES
(1, 'Robert C. Martin'),
(2, 'Andrew Hunt'),
(3, 'Tom Taulli');
GO

INSERT INTO Books (BookId, Title, AuthorId)
VALUES
(1, 'Clean Code', 1),
(2, 'Clean Architecture', 1),
(3, 'The Pragmatic Programmer', 2),
(4, 'Programming Ruby', 2),
(5, 'Artificial Intelligence Basics', 3),
(6, 'AI for Business', 3);
GO

INSERT INTO Categories (CategoryId, CategoryName)
VALUES
(1, 'Programming'),
(2, 'Software Engineering'),
(3, 'Artificial Intelligence'),
(4, 'Business');
GO

INSERT INTO BookCategories (BookId, CategoryId)
VALUES
(1, 1),
(1, 2),
(2, 1),
(2, 2),
(3, 1),
(3, 2),
(4, 1),
(5, 3),
(6, 3),
(6, 4);
GO
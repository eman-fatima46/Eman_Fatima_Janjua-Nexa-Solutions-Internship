USE LibraryDb_Week3;
GO

CREATE TABLE Authors
(
    AuthorId INT PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Books
(
    BookId INT PRIMARY KEY,
    Title NVARCHAR(150) NOT NULL,
    AuthorId INT NOT NULL,

    FOREIGN KEY (AuthorId)
    REFERENCES Authors(AuthorId)
);
GO

CREATE TABLE Categories
(
    CategoryId INT PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE BookCategories
(
    BookId INT NOT NULL,
    CategoryId INT NOT NULL,

    PRIMARY KEY (BookId, CategoryId),

    FOREIGN KEY (BookId)
    REFERENCES Books(BookId),

    FOREIGN KEY (CategoryId)
    REFERENCES Categories(CategoryId)
);
GO
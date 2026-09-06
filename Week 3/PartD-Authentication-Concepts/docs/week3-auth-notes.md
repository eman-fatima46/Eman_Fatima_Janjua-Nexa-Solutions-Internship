# Week 3 - Authentication Concepts

## Authentication

Authentication means proving who a user is. A common example is logging in with an email and password.

## Authorization

Authorization means deciding what an authenticated user is allowed to do. For example, an Admin may be allowed to delete books while a normal user may only be allowed to view them.

## Password Hashing

Passwords should never be stored as plain text. A secure one-way hash should be stored instead so the original password is not directly readable from the database.

## JWT

JWT stands for JSON Web Token. A JWT contains three parts separated by dots:

1. Header
2. Payload
3. Signature

Example JWT:

eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c

### Header

Decoded header:

{
  "alg": "HS256",
  "typ": "JWT"
}

The header identifies the token type and the signing algorithm.

### Payload

Decoded payload:

{
  "sub": "1234567890",
  "name": "John Doe",
  "iat": 1516239022
}

The payload contains claims about the user or token.

### Signature

The signature is the third part of the JWT. It is used by the server to verify that the token has not been changed.

## Claims

Claims are pieces of information contained in the JWT payload, such as user ID, name, email, or role.

## Role-Based Authorization

Role-based authorization restricts actions depending on the user's role. For example, only a user with the Admin role may be allowed to delete a book.

# Practice Exercises

## Exercise 1 - JWT Structure

A JWT contains Header, Payload, and Signature sections separated by dots. I decoded the Header and Payload of an example JWT. The Header showed the token type and signing algorithm. The Payload contained claims such as the subject, name, and issue time. The Signature is used to verify the integrity of the token.

## Exercise 2 - Plain Text Password Risk

If passwords were stored as plain text and the database was leaked, attackers could immediately read every user's password. They could use those passwords to access the application and may also try the same passwords on other websites. Passwords should therefore be securely hashed instead of stored as readable text.

## Exercise 3 - Login and JWT Flow

Login Request
    ↓
Server Checks Password Hash
    ↓
Server Issues JWT
    ↓
Client Stores JWT
    ↓
Client Sends JWT With Next Request
    ↓
Server Validates JWT
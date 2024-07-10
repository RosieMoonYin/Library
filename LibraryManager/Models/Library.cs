using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using LibraryManager.Models;

public class Library 
{
    private List<Book> books;
    private readonly string _filePath;


    public Library(string filePath)
    {
        _filePath = filePath;
        LoadData();
    }

    public IEnumerable<EBook> GetEBooks()
    {
        // Because of inheritance we can treat EBooks as a type of Book (EBook IS A Book)
        return books.OfType<EBook>();
    }

    private void LoadData()
    {
        if (File.Exists(_filePath))
        {
            string jsonData = File.ReadAllText(_filePath);
            books = JsonSerializer.Deserialize<List<Book>>(jsonData) ?? new List<Book>();
        }
        else
        {
            books = new List<Book>();
        }
    }

    private void SaveData()
    {
        string jsonData = JsonSerializer.Serialize(books);
        File.WriteAllText(_filePath, jsonData);
    }

    public void AddBook(Book book)
    {
        books.Add(book);
        SaveData();
    }

    public void RemoveBook(Guid bookId)
    {
        var book = books.FirstOrDefault(b => b.Id == bookId);
        if (book != null)
        {
            books.Remove(book);
            SaveData();
        }
    }

    public IEnumerable<Book> GetBooks()
    {
        return books;
    }
}

using System;
using System.Collections.Generic;
using System.IO;

// Student class
public class Student
{
    public int Id { get; }
    public string FullName { get; }
    public int Score { get; }

    public Student(int id, string name, int score)
    {
        Id = id; FullName = name; Score = score;
    }

    public string GetGrade()
    {
        return Score switch
        {
            >= 80 => "A",
            >= 70 => "B",
            >= 60 => "C",
            >= 50 => "D",
            _ => "F"
        };
    }
}

// Custom exceptions
public class InvalidScoreFormatException : Exception
{
    public InvalidScoreFormatException(string msg) : base(msg) { }
}
public class MissingFieldException : Exception
{
    public MissingFieldException(string msg) : base(msg) { }
}

// Processor class
public class StudentResultProcessor
{
    public List<Student> ReadStudentsFromFile(string inputFilePath)
    {
        var students = new List<Student>();
        using (var reader = new StreamReader(inputFilePath))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');
                if (parts.Length != 3)
                    throw new MissingFieldException($"Line has missing data: {line}");

                if (!int.TryParse(parts[0], out int id))
                    throw new InvalidScoreFormatException($"Invalid ID: {parts[0]}");
                if (!int.TryParse(parts[2], out int score))
                    throw new InvalidScoreFormatException($"Invalid Score: {parts[2]}");

                students.Add(new Student(id, parts[1].Trim(), score));
            }
        }
        return students;
    }

    public void WriteReportToFile(List<Student> students, string outputFilePath)
    {
        using (var writer = new StreamWriter(outputFilePath))
        {
            foreach (var s in students)
            {
                writer.WriteLine($"{s.FullName} (ID: {s.Id}): Score = {s.Score}, Grade = {s.GetGrade()}");
            }
        }
    }
}

public class Program4
{
    public static void Main()
    {
        var processor = new StudentResultProcessor();
        try
        {
            var students = processor.ReadStudentsFromFile("input.txt");
            processor.WriteReportToFile(students, "output.txt");
            Console.WriteLine("Report generated successfully.");
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"File Error: {ex.Message}");
        }
        catch (InvalidScoreFormatException ex)
        {
            Console.WriteLine($"Data Error: {ex.Message}");
        }
        catch (MissingFieldException ex)
        {
            Console.WriteLine($"Data Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected Error: {ex.Message}");
        }
    }
}

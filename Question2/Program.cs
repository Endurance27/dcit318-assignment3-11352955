using System;
using System.Collections.Generic;
using System.Linq;

// Generic repository
public class Repository<T>
{
    private List<T> items = new();

    public void Add(T item) => items.Add(item);
    public List<T> GetAll() => items;
    public T? GetById(Func<T, bool> predicate) => items.FirstOrDefault(predicate);
    public bool Remove(Func<T, bool> predicate)
    {
        var item = items.FirstOrDefault(predicate);
        if (item != null)
        {
            items.Remove(item);
            return true;
        }
        return false;
    }
}

// Patient class
public class Patient
{
    public int Id { get; }
    public string Name { get; }
    public int Age { get; }
    public string Gender { get; }

    public Patient(int id, string name, int age, string gender)
    {
        Id = id; Name = name; Age = age; Gender = gender;
    }

    public override string ToString() => $"{Name} (ID: {Id}, Age: {Age}, Gender: {Gender})";
}

// Prescription class
public class Prescription
{
    public int Id { get; }
    public int PatientId { get; }
    public string MedicationName { get; }
    public DateTime DateIssued { get; }

    public Prescription(int id, int patientId, string med, DateTime date)
    {
        Id = id; PatientId = patientId; MedicationName = med; DateIssued = date;
    }

    public override string ToString() => $"{MedicationName} (Issued: {DateIssued.ToShortDateString()})";
}

// Health System App
public class HealthSystemApp
{
    private Repository<Patient> _patientRepo = new();
    private Repository<Prescription> _prescriptionRepo = new();
    private Dictionary<int, List<Prescription>> _prescriptionMap = new();

    public void SeedData()
    {
        _patientRepo.Add(new Patient(1, "Alice", 28, "Female"));
        _patientRepo.Add(new Patient(2, "Bob", 35, "Male"));

        _prescriptionRepo.Add(new Prescription(1, 1, "Paracetamol", DateTime.Now));
        _prescriptionRepo.Add(new Prescription(2, 1, "Ibuprofen", DateTime.Now));
        _prescriptionRepo.Add(new Prescription(3, 2, "Antibiotic", DateTime.Now));
    }

    public void BuildPrescriptionMap()
    {
        _prescriptionMap = _prescriptionRepo.GetAll()
            .GroupBy(p => p.PatientId)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    public void PrintAllPatients()
    {
        foreach (var p in _patientRepo.GetAll())
            Console.WriteLine(p);
    }

    public void PrintPrescriptionsForPatient(int id)
    {
        if (_prescriptionMap.TryGetValue(id, out var list))
        {
            foreach (var pres in list)
                Console.WriteLine(pres);
        }
        else
        {
            Console.WriteLine("No prescriptions found.");
        }
    }
}

public class Program2
{
    public static void Main()
    {
        var app = new HealthSystemApp();
        app.SeedData();
        app.BuildPrescriptionMap();
        app.PrintAllPatients();
        Console.WriteLine("\nPrescriptions for Patient ID 1:");
        app.PrintPrescriptionsForPatient(1);
    }
}

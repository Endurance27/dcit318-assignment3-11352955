using System;
using System.Collections.Generic;

// Record to represent transaction data
public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

// Interface for processing transactions
public interface ITransactionProcessor
{
    void Process(Transaction transaction);
}

// Concrete processors
public class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Bank Transfer] Processed {transaction.Amount:C} for {transaction.Category}");
    }
}

public class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Mobile Money] Processed {transaction.Amount:C} for {transaction.Category}");
    }
}

public class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[Crypto Wallet] Processed {transaction.Amount:C} for {transaction.Category}");
    }
}

// Base Account class
public class Account
{
    public string AccountNumber { get; }
    public decimal Balance { get; protected set; }

    public Account(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public virtual void ApplyTransaction(Transaction transaction)
    {
        Balance -= transaction.Amount;
        Console.WriteLine($"Transaction applied. New balance: {Balance:C}");
    }
}

// Sealed Savings Account
public sealed class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, decimal initialBalance) 
        : base(accountNumber, initialBalance) { }

    public override void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount > Balance)
        {
            Console.WriteLine("Insufficient funds");
        }
        else
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Transaction applied. Updated balance: {Balance:C}");
        }
    }
}

// Finance application to integrate everything
public class FinanceApp
{
    private List<Transaction> _transactions = new();

    public void Run()
    {
        var account = new SavingsAccount("ACC12345", 1000m);

        var t1 = new Transaction(1, DateTime.Now, 100m, "Groceries");
        var t2 = new Transaction(2, DateTime.Now, 200m, "Utilities");
        var t3 = new Transaction(3, DateTime.Now, 50m, "Entertainment");

        ITransactionProcessor mp = new MobileMoneyProcessor();
        ITransactionProcessor bp = new BankTransferProcessor();
        ITransactionProcessor cp = new CryptoWalletProcessor();

        mp.Process(t1);
        bp.Process(t2);
        cp.Process(t3);

        account.ApplyTransaction(t1);
        account.ApplyTransaction(t2);
        account.ApplyTransaction(t3);

        _transactions.AddRange(new[] { t1, t2, t3 });
    }
}

// Main entry point
public class Program
{
    public static void Main()
    {
        var app = new FinanceApp();
        app.Run();
    }
}

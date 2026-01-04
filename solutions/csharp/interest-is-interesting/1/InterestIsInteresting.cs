static class SavingsAccount
{
    public static float InterestRate(decimal balance) => balance switch
    {
        >= 5000m => 2.475f,
        >= 1000m => 1.621f,
        >= 0m => 0.5f,
        < 0m => 3.213f,
    };

    public static decimal Interest(decimal balance) => balance * (decimal)(InterestRate(balance) / 100);

    public static decimal AnnualBalanceUpdate(decimal balance) => balance += Interest(balance);

    public static int YearsBeforeDesiredBalance(decimal balance, decimal targetBalance)
    {
        var neededYears = 0;
        while(balance < targetBalance)
        {
            neededYears++;
            balance = AnnualBalanceUpdate(balance);
        }

        return neededYears;
    }
}

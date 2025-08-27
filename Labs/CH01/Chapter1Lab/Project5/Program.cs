//get user input

string ccNum;
do
{
    Console.WriteLine("Enter a CC num");
    ccNum = Console.ReadLine();
} while (String.IsNullOrEmpty(ccNum));


//loop through number to hash it
string maskedNum = String.Empty;

for (int index = 0; index < ccNum.Length; index++)
{
    if (ccNum[index] == '-' || Char.IsWhiteSpace(ccNum[index]) || index >= ccNum.Length - 4)
    {
        maskedNum += ccNum[index];
    }
    else
    {
        maskedNum += 'X';
    }
}

//output masked number
Console.WriteLine(maskedNum);
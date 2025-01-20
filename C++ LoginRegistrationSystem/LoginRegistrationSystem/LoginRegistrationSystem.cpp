
#include <iostream>
#include <fstream>
#include <string>

using namespace std;

void registerUser();
void loginUser();
void forgottenPassword();

int main()
{

	int choise;

	while (true)
	{
		cout << "Login and Registration System" << endl;
		cout << "Choise one of options:" << endl;
		cout << "1. Register" << endl;
		cout << "2. Login:" << endl;
		cout << "3. Forgotten Password" << endl;
		cout << "4. Exit" << endl;
		cout << "Enter your choise: ";
		cin >> choise;
		cout << endl;

		switch (choise)
		{
		case 1: registerUser();
			cout << endl;
			break;
		case 2: loginUser();
			cout << endl;
			break;
		case 3: forgottenPassword();
			cout << endl;
			break;
		case 4:
			return 0;

		default:
			cout << "Wrong command! \nTry Again \n"<<endl;
			break;
		}
	}
}

void registerUser()
{
	string userName, password, storedUsername;
	cout << "Register:" << endl;
	cout << "Enter your User Name: ";
	cin >> userName;
	cout << "Enter your Password: ";
	cin >> password;

	ifstream file("user.txt");
	if (file.is_open())
	{
		while (file >> storedUsername)
		{
			if (storedUsername == userName) 
			{
				cout << "UserName is already register, please choise a diffrent user name" << endl;
				file.close();
				return;
			}

		}
		file.close();
	}

	ofstream outFile("user.txt", ios::app);

	if (outFile.is_open())
	{
		outFile << userName << " " << password << endl;
		outFile.close();
		cout << "Register successful!"<<endl;
	}
	else
	{
		cout << "Can't register (file won't open). Try again!" << endl;
	}
}

void loginUser() 
{
	string userName, password, storedUsername, storedPassword;

	cout << "Login:" << endl;
	cout << "Enter your username: ";
	cin >> userName;
	cout << "Enter your password: ";
	cin >> password;

	ifstream file("user.txt");

	if (file.is_open())
	{
		bool loginSuccessful = false;
		while (file >> storedUsername >> storedPassword)
		{
			if (storedUsername == userName && storedPassword == password)
			{
				cout << "Login successful! \nWelcome, " <<userName<<'!'<< endl;
				loginSuccessful = true;
				break;
			}
		}
		file.close();
		
		if(!loginSuccessful)
		{
			cout << "Error! Invalid username and/or password!" << endl;
		}
	}
	else
	{
		cout << "Error: can't open file!" << endl;
	}

}

void forgottenPassword() 
{
	string userName, storedUsername, storedPassword;
	cout << "Forgoten password!" << endl;
	cout << "Enter your user name: ";
	cin >> userName;

	ifstream file("user.txt");

	if (file.is_open())
	{
		bool isFoindUser = false;

		while (file >> storedUsername >> storedPassword)
		{
			if (storedUsername == userName)
			{
				cout << "Your password is: " << storedPassword << endl;
				isFoindUser = true;
				break;
			}
		}

		file.close();

		if (!isFoindUser) 
		{
			cout << "Username not fount, try diffrent user!" << endl;
		}
	}
	else
	{
		cout << "Error! Can't open file!" << endl;
	}
	
}



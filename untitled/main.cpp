#include <iostream>
#include <windows.h>

using namespace std;

int main()
{
    HWND hWnd = FindWindow(0, TEXT("Skype™ - jeff_s86"));
    void* address = (void*)(0x77BEEE54);
    if (!hWnd) {
        MessageBox(0, TEXT("Error cannot find window."), TEXT("Error"), MB_OK | MB_ICONERROR);
        return 1;
    }

    // Get process ID
    DWORD proccess_ID;
    GetWindowThreadProcessId(hWnd, &proccess_ID);

    // Open process
    HANDLE hProcess = OpenProcess(PROCESS_ALL_ACCESS, FALSE, proccess_ID);
    if (!hProcess) {
        MessageBox(0, TEXT("Could not open the process!"), TEXT("Error!"), MB_OK | MB_ICONERROR);
        return 1;
    }

    string r;
    cin >> r;

    if (r == "r") {
        char value[5000];
        ReadProcessMemory(hProcess, address, value, sizeof(value), NULL);
        cout << "Data: " << value << "\n";
    }

    if (r == "w") {
        char value[12] = "aaaaaaaaaa\n";
        DWORD newdatasize = sizeof(value);
        if (WriteProcessMemory(hProcess, address, value, newdatasize, NULL)) {
            MessageBox(NULL, TEXT("WriteProcessMemory worked."), TEXT("Success"), MB_OK + MB_ICONINFORMATION);
        } else {
            MessageBox(NULL, TEXT("Error cannot WriteqProcessMemory!"), TEXT("Error"), MB_OK + MB_ICONERROR);
        }
        CloseHandle(hProcess);
    }

    return 0;
}

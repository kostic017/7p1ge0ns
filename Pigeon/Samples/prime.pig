n = prompt_i("n: ");

if prime(n)
    print("prime");
else
    print("composite");

bool prime(int n) {
    if (n == 1)
        return false;

    i = 2;
    while (i <= n / 2) {
        if (n % i == 0)
            return false;
        i = i + 1;
    }

    return true;
}
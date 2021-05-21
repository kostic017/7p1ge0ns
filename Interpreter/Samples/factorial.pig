n = prompt_i("n: ");
r = factorial(n);
print(r);

int factorial(int n) {
    if (n == 1)
        return 1;
    return n * factorial(n - 1);
}
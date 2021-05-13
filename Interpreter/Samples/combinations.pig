n = 0;
k = 0;

prompt("n: ", n);
prompt("k: ", k);

print(comb(n, k));

int comb(int n, int k) {
    return fact(n) / fact(n - k);
}

int fact(int n) {
    f = 1;
    for i = 1 to n {
        f *= i;
    }
    return f;
}
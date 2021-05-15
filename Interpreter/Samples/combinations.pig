n = prompt_i("n: ");
k = prompt_i("k: ");
print(comb(n, k));

int comb(int n, int k) {
    return fact(n) / (fact(k) * fact(n - k));
}

int fact(int n) {
    f = 1;
    for i = 1 to n {
        f *= i;
    }
    return f;
}
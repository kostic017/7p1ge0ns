int fact(int n) {
    f = 1;
    for i = 1 to n {
        f *= i;
    }
    return f;

}

int comb(int n, int k) {
    return fact(n) / fact(n - k);
}

void main() {
    n = 0;
    k = 0;

    prompt("n: ", n);
    prompt("k: ", k);

    print(comb(n, k));
}
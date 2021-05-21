n = prompt_i("n: ");
r = ones(n);
print(r);

int ones(int n) {
    i = 0;
    while (n > 0) {
        if (n % 2 == 1)
            i += 1;
        n = n / 2;
    }
    return i;
}
// Count ones in binary representation of a number

int ones(int n) {
    i = 0;
    while (n > 0) {
        if (n % 2)
            i++;
        n = n / 2;
    }
    return i;
}

void main() {
    n = 0;
    prompt("n: ", n);
    print(ones(n));
}
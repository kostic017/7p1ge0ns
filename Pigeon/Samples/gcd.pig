﻿a = prompt_i("a: ");
b = prompt_i("b: ");
r = gcd(a, b);
print(r);

int gcd(int a, int b) {
    min = a < b ? a : b;
    for i = min downto 1 {
        if (a % i == 0 && b % i == 0)
            return i;
    }
    return 1;
}
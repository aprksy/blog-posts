# Uniform Initialization
> I'm no expert in this field. This writing is an effort to document my learning process from multiple sources and will be updated when I get more comprehension on the topic. Feel free to correct me, or comment.

Initialization is the act of providing a starting value to a variable or object during its creation. This value determines the initial state of the variable or object in memory and ensures it's in a usable condition before any operations are performed on it.

There are two type of initialization mechanisms, `direct initialization` and `copy initialization`.

1. **Direct initialization** is a mechanism that explicitly use the constructor to construct the value of the variable or object.
2. **Copy initialization**, on the other hand, is a mechanism that use other value or object to construct the value or object.

Now, *what's the uniform term refer to?*
It's uniform by mean of there's no difference between initializing variables of regular types, or custom types (classes, arrays, maps etc.).

## How?

By using the *brace initialization* `{}` (introduced in `C++11`, improved in `C++20`).
```c++
std::string direct {"this is direct initialization"};
std::string copy = {"this is copy initialization"};

int i {10};
int n = {1};

float f {22.7};
float c = {299792458};

int array1[5] {1, 2, 3, 4, 5};
int* array2[3] = new int[3]{10, 20, 30};

std::vector<int> vector1 {0, 1, 2, 3};
std::map<int, std::string> map1 {{1, "one"}, {2, "two"}};

class foo {
    public:
        foo(): _i(0), _f(0.0) {}
        foo(int i, float f): _i(i), _f(f) {}

    private:
        int _i;
        float _f;
}

foo {};
foo {4, 29.3};
```

## Why?
The following are the reasons why you would use it most of the time. 
#### Consistent syntax
It provides same or at least intuitive syntax for variables initialization of various data types.
```c++
// integer
int i {1};
int* iref {12};

// floating points
float f {22.01};
double f {3.1416};

// string
std::string name {"Agung Prakasya"};
std::string email {"agung@example.com"};

// array
int* heights[5] {14, 22, 21, 15, 17};

// map
std::map<std::string, float> dict {{"earth", 67.5}, {"mars", 21.7}, {"venus", 33.8}}

// vector
std::vector<double> distances {12.6, 44.2, 15.7};

// object (from previous section)
foo {};
foo {4, 29.3};

// and so on...
```
#### Doesn't allow narrowing type conversion
The following code won't compile, that is to prevent unintentional typecast which can produce error in further flow of the program.
```c++
// initialize variable d as double with value 10.71 
double d {10.71};

// create variable i as int, with intention to initialize
// with value 10 (integer).
// this won't compile.
int i {d};
```

However, if it's intentionally to cast the value, we can achieve that with the following:
```c++
// initialize variable d as double with value 10.71 
double d {10.71};

// double to int implicit conversion
int i = d;

// modern C++ typecasting - this is the best practice
int i {static_cast<int>(d)};

// C style typecast
int i {(int)d};

// old C++ style typecast
int i {int(d)};
```

#### Fixes most vexing parse
There's a rule in C++ parser that, everytime it finds something considered as function declaration, then treat it as function declaration. 

Now consider the following example:
```c++
class foo {
    public:
        foo() {...}
    private:
        std::vector<int> v(3, 0);
}
```
The code ablve won't compile. The vector `v` initialization will be treated as function declaration, while it's not.

To achieve the vector v initialization we can use some other approach. First, move the initialization into the constructor:
```c++
class foo {
    public:
        foo(): v(3, 0) {}
    private:
        std::vector<int> v;
}
```
second, use the copy initialization:
```c++
class foo {
    public:
        foo() {}
    private:
        std::vector<int> v = std::vector<int>(3, 0);
}
```
third, use the uniform initialization:
```c++
class foo {
    public:
        foo() {}
    private:
        std::vector<int> v {0, 0, 0};
}
```
### Why not?
With all the pros above, still, we need to be aware that there are some cases that we need to not just used it along with other conveniences. Actually the following are just special cases where if all the condition occur, we might need to use another approach.

#### Auto type
While using `auto` in variable declaration is convenience, we might need to sacrifice it when using uniform initialization. Otherwise, we might get the variable type not as intended, or the code doesn't even compile.
```c++
// var_name is of type int
auto var_name {1} 

// var_name is of type std::initializer_list<int>
auto var_name = {2}

// doesn't compile, error variable contains multiple expressions
auto var_name {1, 2, 3}

// var_name is of type std::initializer_list<int>
auto var_name = {2, 3, 4}
```
replacing `auto` with actual data type will solve the problem.

#### Vector type
It's actually more like warning to not mixed up with the old initialization approach like the following code:
```c++
// initialize myvector with {0, 0, 0} elements
std::vector<int> myvector (3, 0);

// this initialization will create myvector with {3, 0} elements 
// instead of {0, 0, 0}
std::vector<int> myvector {3, 0}; 
```

#### The *strongly prefer `std::initializer_list`* constructor
Imagine we have a class with overloaded constructor, first with whatever parameters (in the following example using `int` and `float`) and another one with `std::initializer_list` parameter.
```c++
class foo {
    public:
        // first constructor
        foo(int i, float f) {}
        // second constructor
        foo(std::initializer_list<bool> list) {}
}

foo my_instance {4, 23.5};
```
The code above won't compile. Why? because, foo has the *strongly prefered* constructor that is the second one. Thus, the creation of my_instance won't invoke the first constructor, but instead the second one. In this case, the invocation will attempt to narrow-convert the `int` and `float`, `4` and `23.5` to `bool` which of course will fail.

## Summary
| Key | Value |
| --- | --- |
| Syntax | `Type varName {}` or `Type varName = {}` |
| Introduced in | C++11 |
| Primary Use | Direct & List initialization |
| Flexibility | More versatile |
| Type safety | More type safety |
| Type conversion | Not allow narrowing type conversion |
| **When to use** | Most of the time |
| **When not to use** | see the *Why not?* section |

## Reference
[SYCL 101](https://www.intel.com/content/www/us/en/docs/sycl/introduction/latest/01-uniform-initialization.html)
[]()
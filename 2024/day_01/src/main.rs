use std::{collections::HashMap, ops::AddAssign};

const FILE_INPUT: &str = include_str!("input.txt");

fn main() {
    println!("Task 01: {}", task_01(FILE_INPUT));
    println!("Task 02: {}", task_02(FILE_INPUT));
}

fn task_01(input: &str) -> i32 {
    let (mut a, mut b): (Vec<i32>, Vec<i32>) = input.lines()
        .map(|line| {
            let elements: Vec<i32> = line.split("   ").map(|x| x.parse::<i32>().unwrap()).collect();
            return (*elements.first().unwrap(), *elements.last().unwrap());
        })
        .unzip();

    a.sort();
    b.sort();

    a.into_iter().zip(b)
        .map(|(a,b)| (a - b).abs())
        .sum()
}

fn task_02(input: &str) -> i32 {
    let (a, b): (Vec<i32>, Vec<i32>) = input.lines()
        .map(|line| {
            let elements: Vec<i32> = line.split("   ").map(|x| x.parse::<i32>().unwrap()).collect();
            return (*elements.first().unwrap(), *elements.last().unwrap());
        })
        .unzip();
    let mut map = HashMap::with_capacity(1000);

    for element in b {
        let element_ref = map.get_mut(&element);
        if element_ref.is_none() {
            map.insert(element, 1);
            continue;
        }
        element_ref.unwrap().add_assign(1);
    }

    return a.iter()
        .map(|x| x * map.get(&x).unwrap_or(&0))
        .sum();
}

#[cfg(test)]
mod tests {
    use super::*;

    const TEST_INPUT: &str = "3   4
4   3
2   5
1   3
3   9
3   3
";

    #[test]
    fn task_01_example() {
        assert_eq!(task_01(TEST_INPUT), 11);
    }

    #[test]
    fn task_01_solution() {
        assert_eq!(task_01(FILE_INPUT), 2176849);
    }

    #[test]
    fn task_02_example() {
        assert_eq!(task_02(TEST_INPUT), 31)
    }

    #[test]
    fn task_02_solution() {
        assert_eq!(task_02(FILE_INPUT), 23384288)
    }
}
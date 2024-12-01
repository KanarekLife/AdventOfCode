use std::{collections::HashMap, ops::AddAssign};

const FILE_INPUT: &str = include_str!("input.txt");
const TEST_INPUT: &str = "3   4
4   3
2   5
1   3
3   9
3   3
";

fn main() {
    println!("Task 01: {}", task_01(FILE_INPUT));
    println!("Task 02: {}", task_02(FILE_INPUT));
}

fn task_01(input: &str) -> i32 {
    let lists: (Vec<_>, Vec<_>) = input.lines()
        .flat_map(|line| line.split("   "))
        .enumerate()
        .partition(|(i, _)| i % 2 == 0);

    let mut list_a: Vec<i32> = lists.0
        .iter()
        .map(|(_, x)| x.parse::<i32>().unwrap())
        .collect();
    list_a.sort();

    let mut list_b: Vec<i32> = lists.1
        .iter()
        .map(|(_, x)| x.parse::<i32>().unwrap())
        .collect();
    list_b.sort();

    list_a.into_iter().zip(list_b)
        .map(|(a,b)| (a - b).abs())
        .sum()
}

fn task_02(input: &str) -> i32 {
    let lists: (Vec<_>, Vec<_>) = input.lines()
        .flat_map(|line| line.split("   "))
        .enumerate()
        .partition(|(i, _)| i % 2 == 0);

    let list_b_iter = lists.1
        .iter()
        .map(|(_, x)| x.parse::<i32>().unwrap());
    let mut values = HashMap::new();

    for element in list_b_iter {
        let element_ref = values.get_mut(&element);

        if element_ref.is_none() {
            values.insert(element, 1);
            continue;
        }

        element_ref.unwrap().add_assign(1);
    }
    
    lists.0
        .iter()
        .map(|(_, x)| x.parse::<i32>().unwrap())
        .map(|x| x * values.get(&x).unwrap_or(&0))
        .sum()
}

#[cfg(test)]
mod tests {
    use super::*;

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
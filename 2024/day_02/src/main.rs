use std::ops::AddAssign;

const INPUT: &str = include_str!("input.txt");

fn main() {
    println!("Task 01: {}", task_01(INPUT));
    println!("Task 02: {}", task_02(INPUT));
}

fn task_01(input: &str) -> usize {
    input.lines()
        .filter(|report| {
            let levels: Vec<i32> = report.split_whitespace()
                .map(|level| level.parse::<i32>().unwrap())
                .collect();
            return is_level_correct(&levels);

        })
        .count()
}

fn task_02(input: &str) -> usize {
    input.lines()
        .filter(|report| {
            let levels: Vec<i32> = report.split_whitespace()
                .map(|level| level.parse::<i32>().unwrap())
                .collect();
            
            for i in 0..levels.len() {
                let new_levels: Vec<i32> = levels.iter()
                    .enumerate()
                    .filter(|(j, _)| i != *j)
                    .map(|(_, x)| *x)
                    .collect();

                if is_level_correct(&new_levels) {
                    return true;
                }
            }

            return false;
        })
        .count()
}

fn is_level_correct(levels: &Vec<i32>) -> bool {
    let mut increases = 0;
    let mut decreases = 0;
    for window in levels.windows(2) {
        if (window[1] - window[0]) > 0 {
            increases.add_assign(1);
        } else {
            decreases.add_assign(1);
        }

        if increases > 0 && decreases > 0 {
            return false;
        }

        let difference = (window[1] - window[0]).abs();
        if difference < 1 || difference > 3 {
            return false;
        }
    }

    return true;
}

#[cfg(test)]
mod tests {
    use super::*;

    const TEST_INPUT: &str = "7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9
";

    #[test]
    fn task_01_example() {
        assert_eq!(task_01(TEST_INPUT), 2);
    }

    #[test]
    fn task_01_solution() {
        assert_eq!(task_01(INPUT), 279);
    }

    #[test]
    fn task_02_example() {
        assert_eq!(task_02(TEST_INPUT), 4)
    }

    #[test]
    fn task_02_solution() {
        assert_eq!(task_02(INPUT), 343)
    }
}
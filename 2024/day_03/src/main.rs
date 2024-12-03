use std::ops::AddAssign;

const INPUT: &str = include_str!("input.txt");

fn main() {
    println!("Task 01: {}", task_01(INPUT));
    println!("Task 02: {}", task_02(INPUT));
}

fn task_01(input: &str) -> i32 {
    let mut buffer = String::new();
    let allowed_chars = get_allowed_chars();
    let mut result = 0;

    for c in input.chars() {
        if !c.is_digit(10) && !allowed_chars.contains(&c) {
            buffer.clear();
            continue;
        }

        buffer.push(c);

        if c != ')' {
            continue;
        }

        let parse_result = parse_expr(&buffer);
        if parse_result.is_some() {
            result.add_assign(parse_result.unwrap());
        }
        buffer.clear();
    }

    result
}

fn parse_expr(expr: &str) -> Option<i32> {
    let mut buffer = String::from(expr);
    while !buffer.starts_with("mul") {
        if buffer.len() < 5 {
            return None;
        }

        buffer.remove(0);
    }
    let mut buffer = buffer.replace("mul(", "");
    buffer.pop();
    
    return Some(buffer.split(',').map(|x| x.parse::<i32>().unwrap()).fold(1, |acc, x| acc * x));
}

fn get_allowed_chars() -> Vec<char> {
    let mut vec = Vec::with_capacity(6);
    vec.push('m');
    vec.push('u');
    vec.push('l');
    vec.push('(');
    vec.push(')');
    vec.push(',');
    return vec;
}

fn task_02(input: &str) -> i32 {
    let mut buffer = String::with_capacity(1000);
    let mut enable_mul = true;
    let mut result = 0;

    const DO_INSTR: &str = "do()";
    const DONT_INSTR: &str = "don't()";
    const MUL_OP: &str = "mul";

    for c in input.chars() {
        buffer.push(c);

        if !buffer.ends_with(')') {
            continue;
        }

        if buffer.ends_with(DO_INSTR) {
            enable_mul = true;
            buffer.clear();
            continue;
        } else if buffer.ends_with(DONT_INSTR) {
            enable_mul = false;
            buffer.clear();
            continue;
        }

        if !enable_mul {
            buffer.clear();
            continue;
        }

        let parts: Vec<&str> = buffer.split(MUL_OP).collect();
        if parts.len() <= 1 {
            buffer.clear();
            continue;
        }
        
        let args: Vec<i32> = parts.iter()
                .filter(|slice| slice.starts_with('(') && slice.ends_with(')'))
                .map(|slice| String::from_iter(slice.chars().filter(|c| *c != '(' && *c != ')')))
                .flat_map(|str| str.split(',')
                    .map(|str| str.parse::<i32>())
                    .filter(|x| x.is_ok())
                    .map(|x| x.unwrap())
                    .collect::<Vec<i32>>()
                )
                .collect();

        if args.len() != 2 {
            buffer.clear();
            continue;
        }

        let new_result = args.iter()
            .fold(1, |acc, x| acc * x);

        result.add_assign(new_result);
        buffer.clear();
    }

    return result;
}

#[cfg(test)]
mod tests {
    use super::*;

    const EXAMPLE_INPUT: &str = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";
    const EXAMPLE_2_INPUT: &str = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";

    #[test]
    fn test_01_example() {
        assert_eq!(task_01(EXAMPLE_INPUT), 161);
    }

    #[test]
    fn test_01_solution() {
        assert_eq!(task_01(INPUT), 166357705);
    }

    #[test]
    fn test_02_example() {
        assert_eq!(task_02(EXAMPLE_2_INPUT), 48);
    }

    #[test]
    fn test_02_solution() {
        assert_eq!(task_02(INPUT), 88811886);
    }
}
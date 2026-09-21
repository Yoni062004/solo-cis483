// Assignment 2 — JavaScript Grade Calculator

// Step 1: prompt() returns text, so convert it with Number()
const input = prompt("Enter your score:");
const score = Number(input);

let message;

if (input === null || input.trim() === "" || isNaN(score)) {
  // Challenge: reject empty or non-numeric input
  message = "That is not a number. Please enter a score like 85.";
} else if (score < 0 || score > 100) {
  // Step 2: the score must be between 0 and 100
  message = "Invalid score. Please enter a number between 0 and 100.";
} else {
  // Step 3: decide the letter grade
  let grade;
  if (score >= 90) {
    grade = "A";
  } else if (score >= 80) {
    grade = "B";
  } else if (score >= 70) {
    grade = "C";
  } else if (score >= 60) {
    grade = "D";
  } else {
    grade = "F";
  }
  message = "Your score is " + score + ". Your grade is " + grade + ".";
}

// Step 4: display the result
alert(message);
console.log(message);

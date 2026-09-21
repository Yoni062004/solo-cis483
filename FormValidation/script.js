// Assignment 4 — Form Validation

const form = document.getElementById("grade-form");
const nameInput = document.getElementById("name");
const scoreInput = document.getElementById("score");
const emailInput = document.getElementById("email");
const message = document.getElementById("message");

// Challenge: a basic email format is text@text.text with no spaces
const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

// Requirement 1: listen for the form's submit event
form.addEventListener("submit", (event) => {
  // Requirement 2: stop the browser from submitting the form and reloading the page
  event.preventDefault();

  // Requirement 3: trim the name (removes spaces at the start and end)
  const name = nameInput.value.trim();

  // Requirement 4: convert the score to a number (input values are always text)
  const scoreText = scoreInput.value.trim();
  const score = Number(scoreText);

  const email = emailInput.value.trim();

  // Requirement 5: check required fields and the score range
  const errors = [];

  if (name === "") {
    errors.push("Name is required.");
  }

  if (scoreText === "") {
    errors.push("Score is required.");
  } else if (isNaN(score)) {
    errors.push("Score must be a number.");
  } else if (score < 0 || score > 100) {
    errors.push("Score must be between 0 and 100.");
  }

  if (email === "") {
    errors.push("Email is required.");
  } else if (!emailPattern.test(email)) {
    errors.push("Email must look like name@example.com.");
  }

  if (errors.length > 0) {
    // Requirement 6: show a clear error message when something is invalid
    message.style.color = "red";
    message.textContent = "Please fix the following: " + errors.join(" ");
  } else {
    // Requirement 7: show success feedback when everything is valid
    message.style.color = "green";
    message.textContent = "Success! Thanks, " + name + ". Your score of " + score + " was submitted.";
  }
});

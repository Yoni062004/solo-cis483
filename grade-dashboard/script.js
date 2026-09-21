// Final Mini-Project — Student Grade Dashboard

const form = document.getElementById("grade-form");
const nameInput = document.getElementById("student-name");
const scoreInput = document.getElementById("score");
const feedback = document.getElementById("feedback");
const entryCount = document.getElementById("entry-count");
const averageScore = document.getElementById("average-score");
const clearButton = document.getElementById("clear-button");
const results = document.getElementById("results");
const emptyMessage = document.getElementById("empty-message");

// Each entry is an object: { name: "Alex", score: 85, grade: "B" }
const grades = [];

// Returns the letter grade for a score from 0 to 100
function getGrade(score) {
  if (score >= 90) {
    return "A";
  } else if (score >= 80) {
    return "B";
  } else if (score >= 70) {
    return "C";
  } else if (score >= 60) {
    return "D";
  } else {
    return "F";
  }
}

// Returns a list of problems with the input (an empty list means it is valid)
function validateInput(name, scoreText) {
  const errors = [];
  const score = Number(scoreText);

  if (name === "") {
    errors.push("Enter a student name.");
  }

  if (scoreText === "") {
    errors.push("Enter a score.");
  } else if (isNaN(score)) {
    errors.push("Score must be a number.");
  } else if (score < 0 || score > 100) {
    errors.push("Score must be between 0 and 100.");
  }

  return errors;
}

// Returns the average score, or null when there are no entries
function calculateAverage() {
  if (grades.length === 0) {
    return null;
  }

  let total = 0;
  for (const entry of grades) {
    total += entry.score;
  }
  return total / grades.length;
}

// Shows a message styled as "success" or "error"
function showFeedback(text, type) {
  feedback.textContent = text;
  feedback.className = "feedback " + type;
}

// Rebuilds the result cards and the summary from the grades array
function renderGrades() {
  results.textContent = "";

  for (let i = 0; i < grades.length; i++) {
    const entry = grades[i];

    const card = document.createElement("li");
    card.className = "grade-card";

    const name = document.createElement("span");
    name.className = "student-name";
    name.textContent = entry.name;

    const score = document.createElement("span");
    score.className = "student-score";
    score.textContent = entry.score;

    const letter = document.createElement("span");
    letter.className = "letter grade-" + entry.grade;
    letter.textContent = entry.grade;

    const removeButton = document.createElement("button");
    removeButton.type = "button";
    removeButton.className = "btn btn-remove";
    removeButton.textContent = "Remove";
    removeButton.addEventListener("click", () => {
      grades.splice(i, 1); // removes 1 entry at position i
      showFeedback("Removed " + entry.name + ".", "success");
      renderGrades();
    });

    card.appendChild(name);
    card.appendChild(score);
    card.appendChild(letter);
    card.appendChild(removeButton);
    results.appendChild(card);
  }

  entryCount.textContent = grades.length;

  const average = calculateAverage();
  if (average === null) {
    averageScore.textContent = "—";
  } else {
    averageScore.textContent = average.toFixed(1) + " (" + getGrade(average) + ")";
  }

  emptyMessage.hidden = grades.length > 0;
  clearButton.disabled = grades.length === 0;
}

form.addEventListener("submit", (event) => {
  event.preventDefault();

  const name = nameInput.value.trim();
  const scoreText = scoreInput.value.trim();

  const errors = validateInput(name, scoreText);
  if (errors.length > 0) {
    showFeedback(errors.join(" "), "error");
    return;
  }

  const score = Number(scoreText);
  const entry = { name: name, score: score, grade: getGrade(score) };
  grades.push(entry);

  showFeedback("Added " + name + ": " + score + " (" + entry.grade + ").", "success");
  form.reset();
  nameInput.focus();
  renderGrades();
});

clearButton.addEventListener("click", () => {
  grades.splice(0, grades.length); // removes every entry
  showFeedback("All grades cleared.", "success");
  renderGrades();
});

// Show the empty state when the page first loads
renderGrades();

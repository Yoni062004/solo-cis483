// Assignment 3 — Interactive DOM Webpage

// Task 1: select the heading (and the other elements we need) by id
const heading = document.getElementById("page-title");
const message = document.getElementById("message");
const changeButton = document.getElementById("change-button");
const removeButton = document.getElementById("remove-button");
const output = document.getElementById("output");

// Remembers the paragraph we create, so the remove button can find it
let newParagraph = null;

// Task 4: add a click listener to the button
changeButton.addEventListener("click", () => {
  // Task 2: change the heading's text
  heading.textContent = "The page has changed!";

  // Task 3: change a style property
  heading.style.color = "darkblue";

  // Task 5: create a new element and append it to the output area
  if (newParagraph === null) {
    newParagraph = document.createElement("p");
    newParagraph.textContent = "This paragraph was created by JavaScript.";
    output.appendChild(newParagraph);
    message.textContent = "A new paragraph was added below.";
  } else {
    message.textContent = "The paragraph is already on the page.";
  }
});

// Task 6: a second control that removes the new element
removeButton.addEventListener("click", () => {
  if (newParagraph !== null) {
    newParagraph.remove();
    newParagraph = null;
    message.textContent = "The paragraph was removed.";
  } else {
    message.textContent = "There is nothing to remove yet.";
  }
});

# 1. Working project

## Run project
To run a project read instructions from [README.runproject.md](README.runproject.md)

## APP Overwiew
Real Estate manager that allows to browse properties and with admin role allows to manage property and owners records
- Display all property records with it's owner on the front page 
- Add seeded data on first launch so that page will not be empty
- Display all related property info when property is selected in a modal window
- Implement CRUD operations on existing table(some are redundant/unused and needs to be removed)
- Implement sidebar with Add property/owner buttons, add delete icon on property card to remove it, add option to edit data in the property modal window and submit changes 
- Add authentication
- Implement roles - User with 'User' role can only view property info and user with 'Admin' role can:
- - create properties, owners (from sidebar)
- - delete properties (delete icon on property cards)
- - update properties, owners (in the modal window)
- Implementation covered with unit tests
- Support project run from docker
- Create Github Actions that runs frontend and backed unit tests checks in the PR

(not implemented)
- image support in property cards (it's not working because 'puter' isn't a free image generator, did mistake here)
- search, sorting and filtering
- Logged user cabinet
- A lot of refactoring

# 2. Prompt & workflow log

## Key prompts
Prompts that I used and saved are contained in the PromptExamples folder. I didn't saved the prompts that I used to create a plan.
There are included comments on the context, models/tools
 - [Basic app](./PromptExamples/PromptExample1.md)
 - [JWT Authentication](./PromptExamples/PromptExample2.md)
 - [Backed logic](./PromptExamples/PromptExample3.md)
 - [Frontend logic\UI](./PromptExamples/PromptExample4.md)
 - [Docker](./PromptExamples/PromptExample5.md)

# 3. Insights

## Summary (observations and learnings)

1. Prompts with one purpose and with a detailed explanation, structure etc worked significantly better
2. Long promps and with different purpose in one prompt with no explanation and without custom instructions. I guess it's clear why it doesn't work very well
3. I like ask for a plan from my prompt then redact it and implement from it

## Recommendations

Focus on:
- Divide a task in a subtask as much as you can before prompting it
- Always try to provide more specific information
- Create a custom instructions that describe project context, rules, conventions etc (optional: apply community approved instructions)
Avoid:
- Run prompts without custom instructions
- Prompting a hard task that easily divides into subtasks
- Too generic prompts
- Prompts like 'fix it' with no explanation whatsoever

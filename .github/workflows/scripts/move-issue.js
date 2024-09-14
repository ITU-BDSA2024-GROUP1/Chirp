// Made by ChatGPT
const { graphql } = require("@octokit/graphql");

const moveIssue = async (token, issueId, columnName) => {
  const graphqlWithAuth = graphql.defaults({
    headers: {
      authorization: `token ${token}`,
    },
  });

  // Query your project to get column IDs
  const projectData = await graphqlWithAuth(`
    query {
      user(login: "ITU-BDSA2024-GROUP1") {  // Replace with your org or user login
        projectNext(number: 1) {  // Replace with your project number
          id
          fields(first: 20) {
            nodes {
              id
              name
            }
          }
        }
      }
    }
  `);

  const columnField = projectData.user.projectNext.fields.nodes.find(
    (field) => field.name === "Status"
  );
  
  if (!columnField) {
    throw new Error("Column 'Status' not found in project.");
  }

  const fieldId = columnField.id;

  // Move the issue to the desired column (e.g., "In Progress")
  const result = await graphqlWithAuth(`
    mutation {
      updateProjectNextItemField(input: {
        projectId: "${projectData.user.projectNext.id}",
        itemId: "${issueId}",
        fieldId: "${fieldId}",
        value: "${columnName}"
      }) {
        projectNextItem {
          id
        }
      }
    }
  `);

  console.log(`Issue moved to '${columnName}' column successfully.`);
};

// Grab the token, issue ID, and column name from the command line args
const [_, __, token, issueId, columnName] = process.argv;

moveIssue(token, issueId, columnName)
  .then(() => console.log("Move completed!"))
  .catch((error) => console.error(`Error moving issue: ${error.message}`));

const crypto = require('crypto');
const AWS = require('aws-sdk');
const docClient = new AWS.DynamoDB.DocumentClient();

const userScoresTable = 'userscores';

exports.handler = async (event) => {
    try {
        const requestBody = JSON.parse(event.body);

        const id = crypto.randomUUID();
        const userId = requestBody.userId;
        const score = requestBody.score;
        const date = new Date().toISOString();  //.split('T', 1)[0];

        requestBody.id = id;
        requestBody.userId = userId;
        requestBody.score = score;
        requestBody.createdDate = date;
        
        const params = {
            TableName: userScoresTable,
            Item: requestBody
        };

        await docClient.put(params).promise();
        const body = {
            Operation: 'SAVE SCORES',
            Message: 'SUCCESS',
            Item: requestBody
        }

        return buildResponse(200,body)
    } catch (err) {
        return { error: err }
    }
}

function buildResponse(statusCode, body) {
    return {
        statusCode: statusCode,
        headers: {
            'Access-Control-Expose-Headers': 'Access-Control-Allow-Origin',
            'Access-Control-Allow-Credentials': true,
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Headers': 'Content-Type,X-Amz-Date,Authorization,X-Api-Key,X-Amz-Security-Token',
            'Access-Control-Allow-Methods': 'POST',
            'X-Requested-With': '*'
        },
        body: JSON.stringify(body)
    }
}

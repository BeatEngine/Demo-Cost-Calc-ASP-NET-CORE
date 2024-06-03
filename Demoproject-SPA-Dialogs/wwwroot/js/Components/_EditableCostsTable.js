


function editableTableEventOnChangeCostName(newValue, costId) {
    const restRequestBody = '{"type": "easy-event-request", "values":' +
        '{ "newValue": "' + newValue + '", "costId":"' + costId + '"}  }';

    //-->                       Controller/Method
    sendAndHandleEasyRestEvent("Home/OnCostNameChanged", restRequestBody, true);
}

function editableTableEventOnChangeCostPeriod(newValue, costId) {
    const restRequestBody = '{"type": "easy-event-request", "values":' +
        '{ "newValue": "' + newValue + '", "costId":"' + costId + '"}  }';

    //-->                       Controller/Method
    sendAndHandleEasyRestEvent("Home/OnCostPeriodChanged", restRequestBody, true);
}

function editableTableEventOnChangeCostValue(newValue, costId) {
    const restRequestBody = '{"type": "easy-event-request", "values":' +
        '{ "newValue": "' + newValue + '", "costId":"' + costId + '"}  }';

    //-->                       Controller/Method
    sendAndHandleEasyRestEvent("Home/OnCostValueChanged", restRequestBody, true);
}

function editableTableEventOnRemoveCostrecord(costId) {
    const restRequestBody = '{"type": "easy-event-request", "values":' +
        '{ "costId":"' + costId + '" }  }';

    //-->                       Controller/Method
    sendAndHandleEasyRestEvent("Home/OnRemoveCostrecord", restRequestBody, true);
}

/**
 * Get the input for the sum of costs and set its value to the selected period by name
 * @param {any} periodName The key name of the time period for the result sum of the current costs
 */
function selectCostsSumForPeriodOfTime(periodName) {
    // Get the input for the sum of costs and set its value to the selected period by name
    document.getElementById('output-costs-sum').value = document.getElementById('costs-sum-' + periodName).value;
}

//If site has loaded set the events for the current costrecords table:
function setupEventsForCurrentCostrecords() {
    const bodyCostrecordsTable = document.getElementById('body-costrecords');
    const trListCostrecords = bodyCostrecordsTable.children;

    //Set the onEdit / onHasChanged events etc. for each field of the costrecords
    for (var c = 0; c < trListCostrecords.length; c++) {
        const trElement = trListCostrecords[c];

        const costNameElement = trElement.querySelectorAll('[name="costName"]')[0];
        // If not defined it is the last row for the sum
        if (costNameElement) {
            const costId = costNameElement.getAttribute('data-id');
            const costPeriodElement = trElement.querySelectorAll('[name="costPeriod"]')[0];
            const costValueElement = trElement.querySelectorAll('[name="costValue"]')[0];
            const btnRemove = trElement.querySelectorAll('[name="btnRemove"]')[0];
        
            // Set the event-methods for 'if the content changes' for the above table fields in the current row.
            // input name
            costNameElement.addEventListener('focusout', function () {
                editableTableEventOnChangeCostName(costNameElement.value, costId);
            });
            // select period
            costPeriodElement.addEventListener('change', function () {
                editableTableEventOnChangeCostPeriod(costPeriodElement.value, costId);
            });
            // input value
            costValueElement.addEventListener('focusout', function () {
                editableTableEventOnChangeCostValue(costValueElement.value, costId);
            });
            // button remove
            btnRemove.addEventListener('click', function () {
                editableTableEventOnRemoveCostrecord(costId);
            });

        }
    }

    // Set the event method for selecting a different period of time to update the sum of the current costs
    const sumOfCostsElement = document.getElementById('sum-of-costs-select');
    sumOfCostsElement.addEventListener('change', function () {
        selectCostsSumForPeriodOfTime(sumOfCostsElement.value);
    });


}
setupEventsForCurrentCostrecords();

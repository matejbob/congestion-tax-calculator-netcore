# Congestion Tax Calculator

## Running instance

### Swagger
[https://mbcongestiontax.azurewebsites.net/swagger](https://mbcongestiontax.azurewebsites.net/swagger)

### Sample request body
```
{
  "city": "Gothenburg",
  "vehicle": {
    "category": "Car",
    "isEmergency": false,
    "isDiplomat": false,
    "isMilitary": false,
    "isForeign": false
  },
  "dates": [
    "2013-10-03T08:22:10",
    "2013-10-03T09:00:00",
    "2013-10-04T11:05:05"
  ]
}
```

### Sample response body
```
{
  "total": 21,
  "subcharges": [
    {
      "date": "2013-10-03T00:00:00",
      "subtotal": 13
    },
    {
      "date": "2013-10-04T00:00:00",
      "subtotal": 8
    }
  ]
}
```
## Notes
- All dates and times are expected to be local Gothenburg dates and times. 
- Time zones were not considered.
- *Daylight saving time* was not considered.

## Tests 
To run tests issue following command from the solution directory:
```
dotnet test
```
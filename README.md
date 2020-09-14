# nuget-audit

nuget-audit is a dotnet tool for checking vulnerabilities in your .NET projects.

## Installation

```bash
dotnet tool install -g nuget-audit
```

## Usage

First, you need to set environment variables for using OSS Index for scanning vulnerabilities:
```bash
export NUGET_AUDIT_API_KEY <your_key>
export NUGET_AUDIT_USERNAME <your_username>
```
Then, run:

```bash
nuget-audit --audit-level=(Low|Medium|High|Critical) (path)
```

Examples:

```bash
nuget-audit --audit-level=High .

nuget-audit --audit-level=Low ~/Projects/MyAwesomeProject/
```


## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](https://choosealicense.com/licenses/mit/)
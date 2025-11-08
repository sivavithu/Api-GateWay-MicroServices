# Secret Management Guide

This guide explains how to properly configure secrets for the API Gateway application.

## Overview

The API Gateway requires JWT configuration for authentication. These sensitive values should **never** be committed to version control.

## Required Configuration

The following settings must be configured:

- `AppSettings:issuer` - JWT token issuer
- `AppSettings:Audience` - JWT token audience  
- `AppSettings:Key` - JWT signing key (minimum 32 characters)

## Local Development Setup

For local development, use .NET User Secrets:

### Initial Setup

1. Navigate to the ApiGateway project directory:
   ```bash
   cd ApiGateway
   ```

2. Set your JWT configuration using user secrets:
   ```bash
   dotnet user-secrets set "AppSettings:Key" "REPLACE_WITH_YOUR_JWT_SECRET_KEY_MINIMUM_32_CHARACTERS"
   dotnet user-secrets set "AppSettings:issuer" "REPLACE_WITH_YOUR_ISSUER"
   dotnet user-secrets set "AppSettings:Audience" "REPLACE_WITH_YOUR_AUDIENCE"
   ```

3. Verify your secrets are set:
   ```bash
   dotnet user-secrets list
   ```

### Viewing/Managing Secrets

- **List all secrets**: `dotnet user-secrets list`
- **Remove a secret**: `dotnet user-secrets remove "AppSettings:Key"`
- **Clear all secrets**: `dotnet user-secrets clear`

## Production/Docker Deployment

For production environments, use environment variables:

### Environment Variable Format

Use double underscores (`__`) to represent nested configuration:

```bash
export AppSettings__Key="REPLACE_WITH_YOUR_JWT_SECRET_KEY"
export AppSettings__issuer="REPLACE_WITH_YOUR_ISSUER"
export AppSettings__Audience="REPLACE_WITH_YOUR_AUDIENCE"
```

### Docker Deployment

#### Using docker run:
```bash
docker run -d \
  -e AppSettings__Key="REPLACE_WITH_YOUR_JWT_SECRET" \
  -e AppSettings__issuer="REPLACE_WITH_YOUR_ISSUER" \
  -e AppSettings__Audience="REPLACE_WITH_YOUR_AUDIENCE" \
  -p 8080:8080 \
  api-gateway
```

#### Using docker-compose:
Create a `.env` file based on `.env.example`:
```bash
cp .env.example .env
# Edit .env with your actual values
```

Example `.env` file:
```env
AppSettings__Key=REPLACE_WITH_YOUR_JWT_SECRET_KEY
AppSettings__issuer=REPLACE_WITH_YOUR_ISSUER
AppSettings__Audience=REPLACE_WITH_YOUR_AUDIENCE
```

Update your `docker-compose.yml`:
```yaml
version: '3.8'
services:
  api-gateway:
    image: api-gateway
    env_file:
      - .env
    ports:
      - "8080:8080"
```

Or pass environment variables directly:
```yaml
version: '3.8'
services:
  api-gateway:
    image: api-gateway
    environment:
      - AppSettings__Key=${AppSettings__Key}
      - AppSettings__issuer=${AppSettings__issuer}
      - AppSettings__Audience=${AppSettings__Audience}
    ports:
      - "8080:8080"
```

### Azure Container Apps

Set environment variables in the Azure Portal:
1. Go to your Container App
2. Navigate to "Containers" → "Environment variables"
3. Add:
   - Name: `AppSettings__Key`, Value: (your secret - at least 32 characters)
   - Name: `AppSettings__issuer`, Value: (your issuer)
   - Name: `AppSettings__Audience`, Value: (your audience)

Or use Azure CLI:
```bash
az containerapp update \
  --name api-gateway \
  --resource-group your-rg \
  --set-env-vars \
    "AppSettings__Key=secretref:jwt-key" \
    "AppSettings__issuer=REPLACE_WITH_YOUR_ISSUER" \
    "AppSettings__Audience=REPLACE_WITH_YOUR_AUDIENCE"
```

## Security Best Practices

1. **Never commit secrets** to version control
2. **Use strong keys** - JWT keys should be at least 32 characters
3. **Rotate secrets regularly** in production
4. **Use different secrets** for different environments
5. **Restrict access** to production secrets
6. **Use managed secrets** services like Azure Key Vault for production

## Troubleshooting

### Error: "JWT configuration is missing"

This error occurs when the required configuration values are not set. Follow these steps:

1. **For local development**: Run `dotnet user-secrets list` to verify secrets are configured
2. **For production**: Verify environment variables are set correctly
3. **Check spelling**: Ensure exact key names match (case-sensitive)

### User Secrets Not Working?

Verify the UserSecretsId is set in the `.csproj` file:
```xml
<PropertyGroup>
  <UserSecretsId>0653a0f9-1cd0-4b66-a0f6-ddda7b27ebe5</UserSecretsId>
</PropertyGroup>
```

## Example Configuration

An example configuration file is provided at `appsettings.example.json`. **Do not rename** this to `appsettings.json` as it contains placeholder values only.

## Additional Resources

- [.NET User Secrets Documentation](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [ASP.NET Core Configuration](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
- [Azure Key Vault with .NET](https://docs.microsoft.com/en-us/azure/key-vault/general/overview)

resource "aws_ssm_parameter" "db_user" {
  name  = "/omnifit/db-user"
  type  = "SecureString"
  value = var.db_user
}

resource "aws_ssm_parameter" "db_password" {
  name  = "/omnifit/db-password"
  type  = "SecureString"
  value = var.db_password
}

resource "aws_ssm_parameter" "db_name" {
  name  = "/omnifit/db-name"
  type  = "String"
  value = "omnifit"
}

resource "aws_ssm_parameter" "jwt_key" {
  name  = "/omnifit/jwt-key"
  type  = "SecureString"
  value = var.jwt_key
}

resource "aws_ssm_parameter" "ecr_registry" {
  name  = "/omnifit/ecr-registry"
  type  = "String"
  value = aws_ecr_repository.backend.repository_url
}

resource "aws_ssm_parameter" "frontend_url" {
  name  = "/omnifit/frontend-url"
  type  = "String"
  value = "https://omnifit.lukeatkinson.dev"
}
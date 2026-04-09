terraform {
  required_providers {
    aws = {
      source = "hashicorp/aws"
      version = "~> 6.0"
    }
    cloudflare = {
      source  = "cloudflare/cloudflare"
      version = "~> 5.0"
    }
  }
}

provider "aws" {
  region = var.aws_region
  profile = "terraform"
}

provider "aws" {
  alias = "us_east_1"
  region = "us-east-1"
  profile = "terraform"
}

provider "cloudflare" {
  api_token = var.cloudflare_api_token
}